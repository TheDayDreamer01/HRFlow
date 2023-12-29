﻿using AutoMapper;
using HRIS.Dtos.EmployeeDto;
using HRIS.Exceptions;
using HRIS.Models;
using HRIS.Repositories.DepartmentRepository;
using HRIS.Repositories.EmployeeRepository;
using HRIS.Repositories.PositionRepository;
using HRIS.Utils;
using Microsoft.AspNetCore.JsonPatch;

namespace HRIS.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPositionRepository _positionRepository;

        public EmployeeService(IMapper mapper, IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository, IPositionRepository positionRepository)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _employeeRepository = employeeRepository ??
                throw new ArgumentNullException(nameof(employeeRepository));
            _departmentRepository = departmentRepository ??
                throw new ArgumentNullException(nameof(departmentRepository));
            _positionRepository = positionRepository ??
                throw new ArgumentNullException(nameof(positionRepository));
        }

        public async Task<GetEmployeeRecordDto> GetEmployeeRecord(Guid hrId, Guid employeeId)
        {
            var hr = await _employeeRepository.GetUserById(hrId) ??
                throw new UserNotFoundException("Invalid email address. Please try again.");
            var employee = await _employeeRepository.GetEmployeeRecord(hr, employeeId);
            return _mapper.Map<GetEmployeeRecordDto>(employee);
        }

        public async Task<ICollection<GetEmployeeRecordDto>> GetEmployeeRecords(Guid hrId)
        {
            var hr = await _employeeRepository.GetUserById(hrId) ??
                throw new UserNotFoundException("Invalid email address. Please try again.");

            var employees = await _employeeRepository.GetEmployeeRecords(hr);
            return _mapper.Map<ICollection<GetEmployeeRecordDto>>(employees);
        }

        public async Task<GetEmployeeRecordDto> CreateEmployeeRecord(Guid hrId, CreateEmployeeRecordDto request)
        {
            var hr = await _employeeRepository.GetUserById(hrId) ??
                   throw new UserNotFoundException("Invalid email address. Please try again.");

            var isEmployeeExists = await _employeeRepository.IsEmailExists(request.CompanyEmail);
            if (isEmployeeExists)
            {
                throw new UserExistsException("Employee already exists. Please try again.");
            }

            Password.Encrypt(request.Password, out string passwordHash, out string passwordSalt);

            var selectedDepartment = await _departmentRepository.GetDepartmentByName(hr, request.Name.Trim()) ??
                throw new DepartmentNotFoundException("Department does not exist. Please try again.");
            var selectedPosition = await _positionRepository.GetPositionByName(hr, selectedDepartment, request.Title.Trim()) ??
                throw new PositionNotFoundException("Position does not exist. Please try again.");

            char sex = request.Sex;

            var employee = _mapper.Map<User>(request);
            employee.Role = "Employee";
            employee.Status = "Active";
            employee.CreatedBy = hr.FirstName + " " + hr.LastName;
            employee.TeamId = hr.TeamId;
            employee.DepartmentId = selectedDepartment.Id;
            employee.PositionId = selectedPosition.Id;
            employee.Sex = sex;
            employee.PasswordHash = passwordHash;
            employee.PasswordSalt = passwordSalt;

            var response = await _employeeRepository.CreateEmployeeRecord(employee);
            if (!response)
            {
                throw new Exception("Failed to add new employee record.");
            }
            return _mapper.Map<GetEmployeeRecordDto>(employee);
        }

        public async Task<bool> UpdateEmployeeRecord(Guid hrId, Guid employeeId, JsonPatchDocument<User> request)
        {
            var hr = await _employeeRepository.GetUserById(hrId) ??
                throw new UserNotFoundException("Invalid email address. Please try again.");

            var employee = await _employeeRepository.GetUserById(employeeId) ??
                throw new UserNotFoundException("Employee already exists. Please try again.");

            employee.UpdatedBy = hr.FirstName + " " + hr.LastName;
            employee.UpdatedAt = DateTime.Now;

            return await _employeeRepository.UpdateEmployeeRecord(employee, request);
        }

        public async Task<GetEmployeeRecordDto> UpdateEmployeeRecords(Guid hrId, Guid employeeId, UpdateEmployeeRecordDto request)
        {
            var hr = await _employeeRepository.GetUserById(hrId) ??
                throw new UserNotFoundException("Invalid email address. Please try again.");

            var employee = await _employeeRepository.GetUserById(employeeId) ??
                throw new UserNotFoundException("Employee already exists. Please try again.");

            var dbEmployee = _mapper.Map<User>(request);
            dbEmployee.UpdatedBy = hr.FirstName + " " + hr.LastName;

            var isEmployeeUpdated = await _employeeRepository.UpdateEmployeeRecords(employee, dbEmployee);
            if (!isEmployeeUpdated)
            {
                throw new Exception("Failed to update employee record.");
            }

            return _mapper.Map<GetEmployeeRecordDto>(dbEmployee);
        }

        public Task<GetEmployeeRecordDto> UpdateEmployeePassword(Guid hrId, Guid employeeId, UpdateEmployeePasswordDto request)
        {
            throw new NotImplementedException();
        }
    }
}
