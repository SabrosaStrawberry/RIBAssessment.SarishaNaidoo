import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EmployeesClient, EmployeePersonDTO } from '../../../generate-api';
import { Employee } from '../models/employee';
@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private employeeClient: EmployeesClient) { }

  getEmployees(searchTerm: string | null): Observable<any[]> {
    return this.employeeClient.employeesGet(searchTerm ?? undefined);
  }

  addEmployee(employee: Employee): Observable<any> {
    return this.employeeClient.employeesPost(employee as EmployeePersonDTO);
  }

  updateEmployee(id: number, employee: Employee): Observable<any> {
    return this.employeeClient.employeesPut(id, employee as EmployeePersonDTO);
  }

  deleteEmployee(employeeId: number): Observable<any> {
    return this.employeeClient.employeesDelete(employeeId);
  }
}
