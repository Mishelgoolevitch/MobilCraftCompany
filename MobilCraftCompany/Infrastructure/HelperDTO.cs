﻿using MobilCraftCompany.Domain.Entities;
using MobilCraftCompany.Models;

namespace MobilCraftCompany.Infrastructure
{
    public class HelperDTO
    {
        public static ServiceDTO TransformService(Service entity)
        {
            ServiceDTO entityDTO= new ServiceDTO();
            entityDTO.Id = entity.Id;
            entityDTO.CategoryName = entity.ServiceCategory?.Title;
            entityDTO.Title = entity.Title;
            entityDTO.DescriptionShort = entity.DescriptionShort;
            entityDTO.Description = entity.Description;
            entityDTO.PhotoFileName = entity.Photo;
            entityDTO.Type = entity.Type.ToString();
            return entityDTO;
        }

        public static IEnumerable<ServiceDTO> TransformServices(IEnumerable<Service> entities)
        {
            List<ServiceDTO> entitiesDTO = new List<ServiceDTO>();
            foreach (Service entity in entities)  
            entitiesDTO.Add(TransformService(entity));
            return entitiesDTO;
        }
        //Добавил явное приведение 
        //internal static Service TransformServices(Service entity)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
