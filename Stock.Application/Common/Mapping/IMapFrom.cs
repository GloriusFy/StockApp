using AutoMapper;

namespace Stock.Application.Common.Mapping;

public interface IMapFrom<TEntity>
{
    void Mapping(Profile profile)
    {
        profile.CreateMap(typeof(TEntity), GetType());
    }
}