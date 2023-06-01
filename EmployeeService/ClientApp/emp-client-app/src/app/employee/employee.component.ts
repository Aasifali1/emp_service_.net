import { Component } from '@angular/core';
import { Employee } from '../models/employee';
import { EmployeeService } from '../services/employee.service';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css']
})
export class EmployeeComponent {
  employees: Employee[] = [];

  constructor(private employeeService: EmployeeService) {
    
  }

  ngOnInit() {
    this.employeeService.getEmployees().subscribe(
      {
        next: (response: Employee[]) => {
          this.employees = response;
        },
        error: (error: any) => {
          console.log(error)
        },
        complete: () => { }
      });
  }
}
