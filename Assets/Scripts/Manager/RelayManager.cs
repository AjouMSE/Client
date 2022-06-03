using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firesplash.UnityAssets.SocketIO;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using Utils;

namespace Manager
{
    public class RelayManager : MonoSingleton<RelayManager>
    {
        #region Private variables

        private const int MaxConnectionSize = 2;
        
        private string _playerRelayId;
        private string _allocatedRegion;

        private Guid _hostAllocationId;
        private Guid _clientAllocationId;
        
        private List<Region> _regions;
        
        #endregion
        
        
        #region Public variables

        enum ServiceType
        {
            Host = 0,
            Client = 1
        }

        public string PlayerRelayId => _playerRelayId;
        public string AllocationRegion => _allocatedRegion;

        public List<Region> Regions => _regions;

        #endregion
        
        
        
        #region Private async methods
        
        /// <summary>
        /// First, Authenticate player to UnityService.
        /// </summary>
        private async Task AuthenticatePlayer()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                _playerRelayId = AuthenticationService.Instance.PlayerId;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// Second, Get regions from UnityRelayService.
        /// </summary>
        private async Task GetRelayRegions()
        {
            try
            {
                _regions = await RelayService.Instance.ListRegionsAsync();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// Third, Allocate specific relay service region.
        /// </summary>
        private async Task<(string ipv4address, ushort port, byte[] allocationIdBytes, byte[] key, byte[] connectionData)> AllocateRelayService()
        {
            Allocation allocation = null;
            try
            {
                allocation = await RelayService.Instance.CreateAllocationAsync(
                    MaxConnectionSize, _regions.Count > 0 ? _regions[0].Id : null);
                _hostAllocationId = allocation.AllocationId;
                _allocatedRegion = allocation.Region;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            
            if (allocation != null)
            {
                // Return Allocation information
                RelayServerEndpoint endpoint = allocation.ServerEndpoints.First(e => e.ConnectionType == "dtls");
                return (endpoint.Host, (ushort)endpoint.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);
            }
            
            return ("", 0, null, null, null);
        }

        /// <summary>
        /// Fourth, Create join code to send the client.
        /// </summary>
        private async Task<string> CreateJoinCode()
        {
            string joinCode = null;
            
            try
            {
                joinCode = await RelayService.Instance.GetJoinCodeAsync(_hostAllocationId);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return joinCode;
        }

        /// <summary>
        /// Fifth, Join to host using JoinCode
        /// </summary>
        /// <param name="joinCode"></param>
        private async Task<(string ipv4address, ushort port, byte[] allocationIdBytes, byte[] key, byte[] connectionData, byte[] hostConnectionData)> JoinRelaySession(string joinCode)
        {
            JoinAllocation joinAllocation = null;

            try
            {
                joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                _clientAllocationId = joinAllocation.AllocationId;
                _allocatedRegion = joinAllocation.Region;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            if (joinAllocation != null)
            {
                // Return Allocation information
                RelayServerEndpoint endpoint = joinAllocation.ServerEndpoints.First(e => e.ConnectionType == "dtls");
                return (endpoint.Host, (ushort)endpoint.Port, joinAllocation.AllocationIdBytes, 
                    joinAllocation.Key, joinAllocation.ConnectionData, joinAllocation.HostConnectionData);
            }
            
            return ("", 0, null, null, null, null);
        }
        
        
        private async Task<string> StartService(ServiceType type, string joinCode = default)
        {
            await UnityServices.InitializeAsync();  
            // You must call UnityServices.InitializeAsync before accessing instance of AuthenticationService.
            if(!AuthenticationService.Instance.IsAuthorized) await AuthenticatePlayer();
            if(_regions == null) await GetRelayRegions();

            string ipv4;
            ushort port;
            byte[] allocId, key, connData, hostConnData;
            
            switch (type)
            {
                case ServiceType.Host:
                    var allocServiceResult = await AllocateRelayService();

                    (ipv4, port, allocId, key, connData) = allocServiceResult;
                    NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                        ipv4, port, allocId, key, connData, true);

                    var createCodeResult = await CreateJoinCode();
                    return createCodeResult;

                case ServiceType.Client:
                    (ipv4, port, allocId, key, connData, hostConnData) = await JoinRelaySession(joinCode);
                    NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                        ipv4, port, allocId, key, connData, hostConnData, true);
                    
                    Debug.Log((ipv4, port, allocId, key, connData, hostConnData).ToString());
                    return null;

                default:
                    Debug.LogError("UndefinedServiceTypeException");
                    return null;
            }
        }
        

        #endregion


        #region Public methods

        public async Task<string> StartHost()
        {
            return await StartService(ServiceType.Host);
        }

        public async Task StartClient(string joinCode)
        {
            await StartService(ServiceType.Client, joinCode);
        }
        
        #endregion

        public override void Init()
        {
            throw new NotImplementedException();
        }
    }   
}