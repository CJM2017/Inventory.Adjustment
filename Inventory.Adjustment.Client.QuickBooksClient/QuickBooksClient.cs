// Project      : Inventory Adjusment
// Author       : Connor McCann
// Start Date   : 15 Nov 2018
// Description  : Desktop application for modifying inventory
//                items and pricing information within quickbooks.

namespace Inventory.Adjustment.Client.QuickBooksClient
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;
    using Inventory.Adjustment.Data.Serializable;
    using Interop.QBFC13;
    using System.Xml.Serialization;
    using System.IO;
    using System.Xml;
    using System.Text;

    public class QuickBooksClient : IQuickBooksClient
    {
        private QBSessionManager _manager;
        private bool _disposedValue; // To detect redundant calls
        private bool _connectionOpen;
        private bool _sessionInProgress;
        private bool _debug;

        private string _appName;
        private string _appId;
        private string _country;
        private short _qbsdkMajor;
        private short _qbsdkMinor;

        public QuickBooksClient(string appId, string appName, string country)
        {
            _disposedValue = false;
            _connectionOpen = false;
            _sessionInProgress = false;
            _debug = false;

            _appId = appId;
            _appName = appName;
            _country = country;
            _qbsdkMajor = 0;
            _qbsdkMinor = 0;

            _manager = new QBSessionManager();
        }

        /// <inheritdoc/>
        public async Task<ObservableCollection<InventoryItem>> GetInventory()
        {
            IMsgSetRequest request = CreateRequest();
            request.AppendItemQueryRq();
            IMsgSetResponse queryResponse = await MakeRequestAsync(request).ConfigureAwait(false);

            return await ProcessItemQuery(queryResponse);
        }

        /// <inheritdoc/>
        public async Task<QuickBooksCollection<T>> GetDataFromXML<T>()
        {
            IMsgSetRequest request = CreateRequest();

            switch (typeof(T))
            {
                case Type InventoryItem:
                    request.AppendItemQueryRq();
                    break;
            }

            IMsgSetResponse queryResponse = await MakeRequestAsync(request).ConfigureAwait(false);
            return ProcessQueryAsXML<T>(queryResponse);
        }

        /// <inheritdoc/>
        public InventoryItem CreateInventoryItem(InventoryItem itemToAdd)
        {
            // TODO
            return null;
        }

        /// <inheritdoc/>
        public InventoryItem UpdateInventoryItem(InventoryItem itemToUpdate)
        {
            // TODO
            return null;
        }

        /// <inheritdoc/>
        public InventoryItem DeleteInventoryItem(InventoryItem itemToDelete)
        {
            // TODO
            return null;
        }

        /// <inheritdoc/>
        public async Task OpenConnection()
        {
            if (!_connectionOpen)
            {
                _manager.OpenConnection(_appId, _appName);
                _connectionOpen = true;

                await GetSDKVersionAsync().ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public void CloseConnection()
        {
            this.EndSession();

            if (_connectionOpen)
            {
                _manager.CloseConnection();
                _connectionOpen = false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    this.EndSession();
                    this.CloseConnection();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        /// <summary>
        /// Starts a session with the quickbooks service.
        /// </summary>
        private void BeginSession()
        {
            if (!_sessionInProgress)
            {
                _manager.BeginSession("", ENOpenMode.omDontCare);
                _sessionInProgress = true;
            }
        }

        /// <summary>
        /// Ends the session with the quickbooks service.
        /// </summary>
        private void EndSession()
        {
            if (_sessionInProgress)
            {
                _manager.EndSession();
                _sessionInProgress = false;
            }
        }
        
        /// <summary>
        /// Creates the request body to send to the service.
        /// </summary>
        /// <returns>The generated request</returns>
        private IMsgSetRequest CreateRequest()
        {
            IMsgSetRequest request = _manager.CreateMsgSetRequest(_country, _qbsdkMajor, _qbsdkMinor);
            request.Attributes.OnError = ENRqOnError.roeContinue;
            return request;
        }
         
        /// <summary>
        /// Determines the most recent version supported by 
        /// the quickbooks instance to which we are connecting.
        /// We should always use the latest version supported
        /// by the target instance.
        /// </summary>
        private async Task GetSDKVersionAsync()
        {
            IMsgSetRequest request = _manager.CreateMsgSetRequest(_country, 1, 0);
            request.AppendHostQueryRq();

            IMsgSetResponse queryResponse = await MakeRequestAsync(request).ConfigureAwait(false);
            ProcessSDkQuery(queryResponse);
        }

        /// <summary>
        /// Wrapper around the execution thread.
        /// </summary>
        /// <param name="request">Request message</param>
        /// <returns>Reponse Message</returns>
        private async Task<IMsgSetResponse> MakeRequestAsync(IMsgSetRequest request)
        {
            return await ExecuteRequestAsync(request).ConfigureAwait(false);
        }

        /// <summary>
        /// Execute the request to the quickbooks client asynchronously.
        /// </summary>
        /// <param name="request">Request message</param>
        /// <returns>Response message</returns>
        private async Task<IMsgSetResponse> ExecuteRequestAsync(IMsgSetRequest request)
        {
            return await Task.Run(() =>
            {
                IMsgSetResponse response = null;

                try
                {
                    BeginSession();
                    response = _manager.DoRequests(request);
                }
                catch (Exception ex)
                {
                    throw new QuickBooksClientException(ex.ToString());
                }
                finally
                {
                    EndSession();
                }

                return response;
            });
        }
        
        /// <summary>
        /// Processes the returned query response.
        /// </summary>
        /// <param name="queryResponse"></param>
        private async Task<ObservableCollection<InventoryItem>> ProcessItemQuery(IMsgSetResponse queryResponse)
        {
            ObservableCollection<InventoryItem> inventoryItems = null;
            IResponse response = queryResponse.ResponseList.GetAt(0);
            int statusCode = response.StatusCode;

            if (statusCode == 0)
            {
                if (response.Detail != null)
                {
                    ENResponseType resopnseType = (ENResponseType)response.Type.GetValue();
                    if (resopnseType == ENResponseType.rtItemQueryRs)
                    {
                        IORItemRetList itemList = response.Detail as IORItemRetList;
                        inventoryItems = await ParseItemList(itemList);
                    }
                }
            }

            return inventoryItems;
        }

        private QuickBooksCollection<T> ProcessQueryAsXML<T>(IMsgSetResponse queryResponse)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(QuickBooksCollection<T>));
            QuickBooksCollection<T> result = default(QuickBooksCollection<T>);

            try
            {
                if (_debug)
                {
                    using (FileStream fs = new FileStream(@"C:\Users\Computron\Documents\xml_response.xml", FileMode.OpenOrCreate))
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes(queryResponse.ToXMLString());
                        fs.Write(info, 0, info.Length);
                    }
                }

                using (XmlReader reader = XmlReader.Create(new StringReader(queryResponse.ToXMLString())))
                {
                    reader.MoveToContent();
                    reader.ReadToDescendant("ItemQueryRs");
                    result = (QuickBooksCollection<T>)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new QuickBooksClientException(ex.ToString());
            }

            return result;
        }

        /// <summary>
        /// Parses the returned item list for inventory items.
        /// </summary>
        /// <param name="itemList"></param>
        private async Task<ObservableCollection<InventoryItem>> ParseItemList(IORItemRetList itemList)
        {
            ObservableCollection<InventoryItem> items = new ObservableCollection<InventoryItem>();
            int itemCount = await GetCount("ItemQueryRq");

            for (int i = 0; i < itemCount; i++)
            {
                var item = itemList.GetAt(i);

                if (item.ItemInventoryRet != null && item.ItemInventoryRet.IsActive.GetValue())
                {
                    items.Add(ExtractInventoryItem(item.ItemInventoryRet));
                }
            }

            return items;
        }

        private InventoryItem ExtractInventoryItem(IItemInventoryRet item)
        {
            InventoryItem inventoryItem = new InventoryItem();

            if (item.FullName != null)
            {
                inventoryItem.Name = item.FullName.GetValue();
            }

            if (item.ManufacturerPartNumber != null)
            {
                inventoryItem.Code = item.ManufacturerPartNumber.GetValue();
            }

            if (item.QuantityOnHand != null)
            {
                inventoryItem.Stock = (int)item.QuantityOnHand.GetValue();
            }

            if (item.SalesPrice != null)
            {
                inventoryItem.BasePrice = item.SalesPrice.GetValue();
            }

            return inventoryItem;
        }

        /// <summary>
        /// Creates the request to count the number of items in the response list.
        /// </summary>
        /// <param name="requestType"></param>
        /// <returns></returns>
        private async Task<int> GetCount(string requestType)
        {
            IMsgSetRequest request = CreateRequest();
            
            switch (requestType)
            {
                case "ItemQueryRq":
                    IItemQuery itemQuery =  request.AppendItemQueryRq();
                    itemQuery.metaData.SetValue(ENmetaData.mdMetaDataOnly);
                    break;
            }

            IMsgSetResponse queryResponse = await MakeRequestAsync(request).ConfigureAwait(false);

            return ParseRsForCount(queryResponse);
        }

        /// <summary>
        /// Gets the number of items in the meta response.
        /// </summary>
        /// <param name="queryResponse"></param>
        /// <returns>Int</returns>
        private int ParseRsForCount(IMsgSetResponse queryResponse)
        {
            IResponse response = queryResponse.ResponseList.GetAt(0);
            return response.retCount;
        }

        /// <summary>
        /// Requests and sets the latest quickbooks sdk version.
        /// </summary>
        /// <param name="queryResponse"></param>
        private void ProcessSDkQuery(IMsgSetResponse queryResponse)
        {
            IResponse response = queryResponse.ResponseList.GetAt(0);
            IHostRet HostResponse = (IHostRet)response.Detail;

            IBSTRList supportedVersions = HostResponse.SupportedQBXMLVersionList;
            ParseSDK(supportedVersions);
        }

        /// <summary>
        /// Parses the reponse for the latest sdk version number.
        /// </summary>
        /// <param name="supportedVersions"></param>
        private void ParseSDK(IBSTRList supportedVersions)
        {
            string svers = string.Empty;
            double version = 0.0;
            double latestVersion = 0.0;

            for (int i = 0; i < supportedVersions.Count; i++)
            {
                svers = supportedVersions.GetAt(i);
                version = Convert.ToDouble(svers);
                latestVersion = (version > latestVersion) ? version : latestVersion;
            }

            _qbsdkMajor = (short)Math.Floor(latestVersion);
            _qbsdkMinor = (short)((latestVersion * 100) - (_qbsdkMajor * 100));
        }
    }
}
