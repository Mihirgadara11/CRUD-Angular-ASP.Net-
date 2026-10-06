import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { EmployeeService } from '../services/employee.service';
import { Employee } from '../models/employee';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './employee-list.component.html',
  styleUrl: './employee-list.component.scss'
})
export class EmployeeListComponent implements OnInit {
  employees: Employee[] = [];
  searchTerm = '';
  isLoading = false;
  errorMessage = '';
  employeeToDelete: Employee | null = null;

  constructor(private employeeService: EmployeeService) {}

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.employeeService.getAll(this.searchTerm).subscribe({
      next: (data) => {
        this.employees = data;
        this.isLoading = false;
      },
      error: (err: any) => {
        this.errorMessage = 'Failed to load employees. Please try again.';
        this.isLoading = false;
        console.error(err);
      }
    });
  }

  onSearchChange(): void {
    this.loadEmployees();
  }

  confirmDelete(employee: Employee): void {
    this.employeeToDelete = employee;
  }

  cancelDelete(): void {
    this.employeeToDelete = null;
  }

  deleteConfirmed(): void {
    if (!this.employeeToDelete) return;
    const id = this.employeeToDelete.employeeId;
    this.employeeService.delete(id).subscribe({
      next: () => {
        this.employees = this.employees.filter(e => e.employeeId !== id);
        this.employeeToDelete = null;
      },
      error: (err: any) => {
        this.errorMessage = 'Failed to delete employee.';
        console.error(err);
      }
    });
  }
}
