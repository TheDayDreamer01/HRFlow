﻿using HRIS.Dtos.EmployeeDto;
using HRIS.Dtos.PositionDto;
using HRIS.Models;

namespace HRIS.Dtos.DepartmentDto
{
    public class GetDepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Manager { get; set; } = string.Empty;
        public string Assistant { get; set; } = string.Empty;
        public ICollection<User> Supervisors { get; set; } = new List<User>();
        public Guid? TeamId { get; set; }
        public List<GetPositionDto> Positions { get; set; } = new List<GetPositionDto>();
        public List<GetEmployeeRecordDto> Users { get; set; } = new List<GetEmployeeRecordDto>();
    }
}
