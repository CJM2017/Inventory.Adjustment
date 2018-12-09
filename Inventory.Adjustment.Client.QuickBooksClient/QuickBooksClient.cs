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
    using System.Collections;

    public class QuickBooksClient : IQuickBooksClient
    {
        private QBSessionManager _manager;
        private bool _disposedValue; // To detect redundant calls
        private bool _connectionOpen;
        private bool _sessionInProgress;

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
            // TODO
            IMsgSetRequest request = CreateRequest();
            request.Attributes.OnError = ENRqOnError.roeContinue;
            request.AppendItemQueryRq();

            IMsgSetResponse queryResponse = await MakeRequestAsync(request).ConfigureAwait(false);
            ProcessItemQuery(queryResponse);

            return null;
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

        /// <summary>
        /// Opens the connection to the quickbooks service.
        /// </summary>
        public async Task OpenConnection()
        {
            if (!_connectionOpen)
            {
                _manager.OpenConnection(_appId, _appName);
                _connectionOpen = true;

                await GetSDKVersionAsync().ConfigureAwait(false);
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
                    EndSession();
                    CloseConnection();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        /// <summary>
        /// Wrapper for test thread.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TestConnectionAsync()
        {
            return await RunTestsAsync().ConfigureAwait(false);
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
        /// Closes the connection to the quickbooks service.
        /// </summary>
        private void CloseConnection()
        {
            if (_connectionOpen)
            {
                _manager.CloseConnection();
                _connectionOpen = false;
            }
        }
        
        /// <summary>
        /// Creates the request body to send to the service.
        /// </summary>
        /// <returns>The generated request</returns>
        private IMsgSetRequest CreateRequest()
        {
            return _manager.CreateMsgSetRequest(_country, _qbsdkMajor, _qbsdkMinor);
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
            return await ExecuteRequestAsync(request);
        }

        /// <summary>
        /// Execute the request to the quickbooks client asynchronously.
        /// </summary>
        /// <param name="request">Request message</param>
        /// <returns>Response message</returns>
        private async Task<IMsgSetResponse> ExecuteRequestAsync(IMsgSetRequest request)
        {
            IMsgSetResponse response = null;
            QuickBooksClientException exception = null;

            Task.Run(() =>
            {
                try
                {
                    BeginSession();
                    response = _manager.DoRequests(request);
                }
                catch (Exception ex)
                {
                    exception = new QuickBooksClientException(ex.ToString());
                }
                finally
                {
                    EndSession();
                }
            }).GetAwaiter().GetResult();

            if (exception != null)
            {
                throw exception;
            }

            return response;
        }
        
        private void ProcessItemQuery(IMsgSetResponse queryResponse)
        {
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
                    }
                }
            }
        }

        /// <summary>
        /// Parses the reponse for the latest sdk version number.
        /// </summary>
        /// <param name="queryResponse"></param>
        private void ProcessSDkQuery(IMsgSetResponse queryResponse)
        {
            IResponse response = queryResponse.ResponseList.GetAt(0);
            IHostRet HostResponse = (IHostRet)response.Detail;

            IBSTRList supportedVersions = HostResponse.SupportedQBXMLVersionList;

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
            _qbsdkMinor = (short)(Math.Ceiling(latestVersion * 100) - (latestVersion * 100));
        }

        /// <summary>
        /// Async test to determine if we can communicate
        /// with the quickbooks client.
        /// </summary>
        /// <returns>True if the connection was successful</returns>
        private async Task<bool> RunTestsAsync()
        {
            bool result = true;

            Task.Run(async () =>
            {
                try
                {
                    await OpenConnection();
                    BeginSession();
                    EndSession();
                    CloseConnection();
                }
                catch (Exception ex)
                {
                    result = false;
                } 
            }).GetAwaiter().GetResult();

            return result;
        }
    }
}
