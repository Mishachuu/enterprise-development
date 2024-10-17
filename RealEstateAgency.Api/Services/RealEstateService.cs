using AutoMapper;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Interface;

namespace RealEstateAgency.Api.Services;

public class RealEstateService(IRepository<RealEstate, int> realEstateRepository, IMapper mapper)
{
    public async Task<List<RealEstateDto>> GetAllRealEstates()
    {
        var realEstates = await realEstateRepository.GetAsList();
        return mapper.Map<List<RealEstateDto>>(realEstates);
    }

    public async Task<List<RealEstateDto>> GetRealEstatesByPredicate(Func<RealEstate, bool> predicate)
    {
        var realEstates = await realEstateRepository.GetAsList(predicate);
        return mapper.Map<List<RealEstateDto>>(realEstates);
    }

    public async Task AddRealEstate(RealEstateDto realEstateDto)
    {
        var realEstate = mapper.Map<RealEstate>(realEstateDto);
        await realEstateRepository.Add(realEstate);
    }

    public async Task UpdateRealEstate(int id, RealEstateDto realEstateDto)
    {
        var allRealEstate = await realEstateRepository.GetAsList();
        if (!allRealEstate.Any(l => l.Id == id))
        {
            throw new ArgumentException("Неправильный ID");
        }
        var realEstate = mapper.Map<RealEstate>(realEstateDto);
        realEstate.Id = id;
        await realEstateRepository.Update(realEstate);
    }

    public async Task DeleteRealEstate(int realEstateId)
    {
        await realEstateRepository.Delete(realEstateId);
    }

}
