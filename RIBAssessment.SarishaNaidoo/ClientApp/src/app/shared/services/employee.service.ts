import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Client, EmployeePersonDTO } from '../../../generate-api';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private employeeClient: Client) {}

  getEmployees(): Observable<any[]> {
    return this.employeeClient.getEmployees();
  }

  addEmployee(employee: EmployeePersonDTO): Observable<any> {
    return this.employeeClient.employeePOST(employee);
  }

  updateEmployee(id: number, employee: EmployeePersonDTO): Observable<any> {
    return this.employeeClient.employeePUT(id,employee);
  }

  deleteEmployee(employeeId: number): Observable<any> {
    return this.employeeClient.employeeDELETE(employeeId);
  }
}
