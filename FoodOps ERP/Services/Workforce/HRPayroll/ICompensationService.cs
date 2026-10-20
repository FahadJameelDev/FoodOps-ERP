using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;

namespace FoodOpsERP.Services.Workforce.HRPayroll;

public interface ICompensationService
{
    // Salary Structures
    Task<List<SalaryStructureViewModel>> GetAllStructuresAsync();
    Task<SalaryStructureViewModel?> GetStructureByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateStructureAsync(SalaryStructureViewModel model, int userId);
    Task<(bool Success, string Message)> UpdateStructureAsync(SalaryStructureViewModel model, int userId);
    Task<(bool Success, string Message)> ToggleStructureActiveAsync(int id, int userId);

    // Employee Salaries
    Task<List<EmployeeSalaryViewModel>> GetAllEmployeeSalariesAsync(int? employeeId = null);
    Task<EmployeeSalaryViewModel?> GetEmployeeSalaryByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateEmployeeSalaryAsync(EmployeeSalaryViewModel model, int userId);
    Task<(bool Success, string Message)> UpdateEmployeeSalaryAsync(EmployeeSalaryViewModel model, int userId);
}