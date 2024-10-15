using AutoMapper;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Interface;

namespace RealEstateAgency.Api.Services;

public class RealEstateService(IRepository<RealEstate, int> realEstateRepository, IMapper mapper)
{
    private readonly IRepository<RealEstate, int> _realEstateRepository = realEstateRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<RealEstateDto>> GetAllRealEstates()
    {
        var realEstates = await _realEstateRepository.GetAsList();
        return _mapper.Map<List<RealEstateDto>>(realEstates);
    }

    public async Task<List<RealEstateDto>> GetRealEstatesByPredicate(Func<RealEstate, bool> predicate)
    {
        var realEstates = await _realEstateRepository.GetAsList(predicate);
        return _mapper.Map<List<RealEstateDto>>(realEstates);
    }

    public async Task AddRealEstate(RealEstateDto realEstateDto)
    {
        var realEstate = _mapper.Map<RealEstate>(realEstateDto);
        await _realEstateRepository.Add(realEstate);
    }

    public async Task UpdateRealEstate(RealEstateDto realEstateDto)
    {
        var realEstate = _mapper.Map<RealEstate>(realEstateDto);
        await _realEstateRepository.Update(realEstate);
    }

    public async Task DeleteRealEstate(int realEstateId)
    {
        await _realEstateRepository.Delete(realEstateId);
    }

}
