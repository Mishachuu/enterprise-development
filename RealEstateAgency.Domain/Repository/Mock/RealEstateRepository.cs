using RealEstateAgency.Domain.Interface;

namespace RealEstateAgency.Domain.Repository.Mock
{
    public class RealEstateRepository : IRepository<RealEstate, int>
    {
        private static readonly List<RealEstate> _realEstates = [];
        private static int _currentId;

        public async Task<List<RealEstate>> GetAsList()
        {
            return await Task.FromResult(_realEstates);
        }

        public async Task<List<RealEstate>> GetAsList(Func<RealEstate, bool> predicate)
        {
            return await Task.FromResult(_realEstates.Where(predicate).ToList());
        }

        public async Task Add(RealEstate newRecord)
        {
            newRecord.Id = _currentId++;
            await Task.Run(() => _realEstates.Add(newRecord));
        }

        public async Task Delete(int key)
        {
            var realEstate = _realEstates.FirstOrDefault(r => r.Id == key);
            if (realEstate != null)
            {
                await Task.Run(() => _realEstates.Remove(realEstate));
            }
        }

        public async Task Update(RealEstate newValue)
        {
            var realEstate = _realEstates.FirstOrDefault(r => r.Id == newValue.Id);
            if (realEstate != null)
            {
                await Task.Run(() =>
                {
                    realEstate.Square = newValue.Square;
                    realEstate.NumberOfRooms = newValue.NumberOfRooms;
                    realEstate.Type = newValue.Type;
                    realEstate.Address = newValue.Address;
                });
            }
        }
    }
}
