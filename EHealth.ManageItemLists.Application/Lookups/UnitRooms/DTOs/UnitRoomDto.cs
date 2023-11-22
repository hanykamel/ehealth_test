using EHealth.ManageItemLists.Application.Lookups.Categories.DTOs;
using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.UnitRooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Application.Lookups.UnitRooms.DTOs
{
    public class UnitRoomDto
    {
        public int Id { get; set; }
        public string NameAr { get; private set; }
        public string NameEN { get; private set; }
        public string? DefinitionAr { get; private set; }
        public string? DefinitionEN { get; private set; }

        public static UnitRoomDto FromUnitRoom(UnitRoom input) =>
        input is not null ? new UnitRoomDto
        {
            Id = input.Id,
            NameAr =input.NameAr,
            NameEN =input.NameEN,
            DefinitionAr =input.DefinitionAr,
            DefinitionEN =input.DefinitionEN
        } : null;
    }
}
