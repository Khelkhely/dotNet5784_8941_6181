using BlApi;
using BO;

namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = Factory.Get;
    public void CreateSchedule()
    {
        throw new NotImplementedException();
    }

    public Milestone Read(int id)
    {
        throw new NotImplementedException();
    }

    public Milestone Update(int id)
    {
        throw new NotImplementedException();
    }
}
