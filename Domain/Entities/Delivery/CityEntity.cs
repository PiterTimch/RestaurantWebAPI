﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Delivery;

[Table("tblCities")]
public class CityEntity : BaseEntity<long>
{
    public string Name { get; set; } = string.Empty;

    public ICollection<PostDepartmentEntity>? PostDepartments { get; set; }
    public ICollection<DeliveryInfoEntity>? DeliveryInfos { get; set; }
}
