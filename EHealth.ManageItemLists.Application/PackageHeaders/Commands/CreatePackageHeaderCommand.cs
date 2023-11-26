using EHealth.ManageItemLists.Application.PackageHeaders.DTOs;
using MediatR;

namespace EHealth.ManageItemLists.Application.PackageHeaders.Commands
{
    public record CreatePackageHeaderCommand(CreatePackageHeaderDto CreatePackageHeaderDto) : IRequest<Guid>;
}
