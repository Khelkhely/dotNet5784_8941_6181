using BlApi;
using BO;

namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = Factory.Get;
    public void Create(Engineer engineer)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Engineer Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Engineer> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(Engineer engineer)
    {
        throw new NotImplementedException();
    }
}
