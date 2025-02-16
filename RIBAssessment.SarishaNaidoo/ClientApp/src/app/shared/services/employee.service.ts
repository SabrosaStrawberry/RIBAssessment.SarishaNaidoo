import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EmployeesClient, EmployeePersonDTO } from '../../../generate-api';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private employeeClient: EmployeesClient) { }

  getEmployees(searchTerm: string | null): Observable<any[]> {
    return this.employeeClient.employeesGet(searchTerm ?? undefined);
  }

  addEmployee(employee: EmployeePersonDTO): Observable<any> {
    return this.employeeClient.employeesPost(employee);
  }

  updateEmployee(id: number, employee: EmployeePersonDTO): Observable<any> {
    return this.employeeClient.employeesPut(id, employee);
  }

  deleteEmployee(employeeId: number): Observable<any> {
    return this.employeeClient.employeesDelete(employeeId);
  }
}
