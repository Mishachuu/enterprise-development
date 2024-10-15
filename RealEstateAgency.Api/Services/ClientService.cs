using AutoMapper;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Interface;

namespace RealEstateAgency.Api.Services;

public class ClientService(IRepository<Client, int> clientRepository, IMapper mapper)
{
    private readonly IRepository<Client, int> _clientRepository = clientRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<ClientDto>> GetAllClients()
    {
        var clients = await _clientRepository.GetAsList();
        return _mapper.Map<List<ClientDto>>(clients);
    }

    public async Task<List<ClientDto>> GetClientsByPredicate(Func<Client, bool> predicate)
    {
        var clients = await _clientRepository.GetAsList(predicate);
        return _mapper.Map<List<ClientDto>>(clients);
    }

    public async Task AddClient(ClientDto clientDto)
    {
        var client = _mapper.Map<Client>(clientDto);
        await _clientRepository.Add(client);
    }

    public async Task DeleteClient(int clientId)
    {
        await _clientRepository.Delete(clientId);
    }

    public async Task UpdateClient(ClientDto clientDto)
    {
        var client = _mapper.Map<Client>(clientDto);
        await _clientRepository.Update(client);
    }
}
