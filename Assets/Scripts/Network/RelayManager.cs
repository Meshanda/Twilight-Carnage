using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayManager : GenericSingleton<RelayManager>
{
    private string _joinCode;
    private string _ip;
    private int _port;
    private byte[] _connectionData;
    private System.Guid _allocationId;


    public string Ip { get => _ip; }
    public int Port { get => _port; }
    public string GetAllocationId()
    {
        return _allocationId.ToString();
    }

    public string GetConnectionData()
    {
        return _connectionData.ToString();
    }
    
    public async Task<string> CreateRelay(int maxConnection)
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnection);
        _joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.First(conn => conn.ConnectionType == "dtls");
        _ip = dtlsEndpoint.Host;
        _port = dtlsEndpoint.Port;

        _allocationId = allocation.AllocationId;
        _connectionData = allocation.ConnectionData;

        return _joinCode;
    }

    public async Task<bool> JoinRelay(string joinCode)
    {
        _joinCode = joinCode;
        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        
        RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.First(conn => conn.ConnectionType == "dtls");
        _ip = dtlsEndpoint.Host;
        _port = dtlsEndpoint.Port;

        _allocationId = allocation.AllocationId;
        _connectionData = allocation.ConnectionData;

        return true;
    }
}
