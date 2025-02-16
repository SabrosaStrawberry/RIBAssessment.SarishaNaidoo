import { Component, OnInit, ViewChild } from '@angular/core';
import { EmployeeAddEditDialogComponent } from '../employee-add-edit-dialog/employee-add-edit-dialog.component';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { EmployeeService } from '../../shared/services/employee.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { debounceTime, distinctUntilChanged, map, Observable, startWith } from 'rxjs';

@Component({
  selector: 'app-employee-overview',
  templateUrl: './employee-overview.component.html',
  styleUrl: './employee-overview.component.css',
  standalone: false
})
export class EmployeeOverviewComponent implements OnInit {
  displayedColumns: string[] = [
    'firstName', 'lastName', 'employeeNumber', 'birthDate', 'employedDate', 'terminatedDate', 'action'
  ];

  dataSource = new MatTableDataSource<any>([]);
  filteredOptions!: Observable<string[]>;
  formGroup!: FormGroup;
  employeeList: employee[] = [];
  loading: boolean = false;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private dialog: MatDialog,
    private employeeService: EmployeeService,
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.initForm();
    this.getEmployeeList();

    // Custom filter predicate for table filtering
    this.dataSource.filterPredicate = (data, filter) => {
      const searchStr = filter.trim().toLowerCase();
      return (
        data.firstName.toLowerCase().includes(searchStr) ||
        data.lastName.toLowerCase().includes(searchStr)
      );
    };
  }

  initForm() {
    this.formGroup = this.formBuilder.group({
      employee: ['']
    });

    // Setup value changes for filtering
    this.filteredOptions = this.formGroup.get('employee')!.valueChanges.pipe(
      startWith(''),
      debounceTime(300),
      distinctUntilChanged(),
      map(value => this.filterEmployees(value || ''))
    );

    // Update table filter when an employee is selected
    this.formGroup.get('employee')!.valueChanges.subscribe(value => {
      this.applyFilter(value);
    });
  }

  /** Fetch Employee List */
  getEmployeeList(searchTerm?: string) {
    this.loading = true;
    this.employeeService.getEmployees(searchTerm ?? null).subscribe({
      next: (res) => {
        setTimeout(() => {
          this.employeeList = res;

          this.dataSource.data = [...res]; // Ensuring proper data update


          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
        });
      },
      error: (err) => console.error(err),
      complete: () => this.loading = false
    });
  }


  /** Filters Employees for Autocomplete */
  filterEmployees(value: string): string[] {
    if (!value.trim()) {
      return []; // Keep autocomplete empty until user types
    }

    const filterValue = value.toLowerCase();

    return this.employeeList
      .filter(emp =>
        emp.firstName.toLowerCase().includes(filterValue) ||
        emp.lastName.toLowerCase().includes(filterValue) ||
        emp.employeeNumber.toLowerCase().includes(filterValue)
      )
      .map(emp => {
        // If searching by employee number, return only employee number
        if (emp.employeeNumber.toLowerCase().includes(filterValue)) {
          return emp.employeeNumber;
        }
        // therwise, return "FirstName LastName"
        return `${emp.firstName} ${emp.lastName}`;
      });
  }
  /** Filters the Employee Table */
  applyFilter(value: string) {
    this.getEmployeeList(value);

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  /** Opens Add/Edit Employee Dialog */
  openAddEditEmployeeDialog(data: any = null) {
    const dialogRef = this.dialog.open(EmployeeAddEditDialogComponent, { data });

    dialogRef.afterClosed().subscribe(val => {
      if (val) {
        this.getEmployeeList();
      }
    });
  }

  /** Delete Employee */
  deleteEmployee(id: number) {
    if (confirm("Are you sure you want to delete this employee?")) {
      this.employeeService.deleteEmployee(id).subscribe({
        next: () => {
          alert('Employee deleted!');
          this.getEmployeeList();
        },
        error: (err) => console.error(err),
      });
    }
  }
}

interface employee {
  firstName: string;
  lastName: string;
  employeeNumber: string;
  employedDate: Date;
  terminatedDate: Date;
  fullName: string;
}