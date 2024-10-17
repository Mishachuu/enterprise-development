using AutoMapper;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Interface;

namespace RealEstateAgency.Api.Services;

public class ClientService(IRepository<Client, int> clientRepository, IMapper mapper)
{

    public async Task<List<ClientDto>> GetAllClients()
    {
        var clients = await clientRepository.GetAsList();
        return mapper.Map<List<ClientDto>>(clients);
    }

    public async Task<List<ClientDto>> GetClientsByPredicate(Func<Client, bool> predicate)
    {
        var clients = await clientRepository.GetAsList(predicate);
        return mapper.Map<List<ClientDto>>(clients);
    }

    public async Task AddClient(ClientDto clientDto)
    {
        var client = mapper.Map<Client>(clientDto);
        await clientRepository.Add(client);
    }

    public async Task DeleteClient(int clientId)
    {
        await clientRepository.Delete(clientId);
    }

    public async Task UpdateClient(int id, ClientDto clientDto)
    {
        var allClient = await clientRepository.GetAsList();
        if (!allClient.Any(l => l.ClientId == id))
        {
            throw new ArgumentException("Неправильный ID");
        }
        var client = mapper.Map<Client>(clientDto);
        client.ClientId = id;
        await clientRepository.Update(client);

    }
}
