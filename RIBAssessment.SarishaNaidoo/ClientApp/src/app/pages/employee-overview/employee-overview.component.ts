import { Component, OnInit, ViewChild } from '@angular/core';
import { EmployeeAddEditDialogComponent } from '../employee-add-edit-dialog/employee-add-edit-dialog.component';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { EmployeeService } from '../../shared/services/employee.service';
import { FormControl } from '@angular/forms';
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

  dataSource!: MatTableDataSource<any>;
  filteredOptions!: Observable<string[]>;
  searchControl = new FormControl();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private dialog: MatDialog, private employeeService: EmployeeService) { }

  ngOnInit(): void {
    this.getEmployeeList();
    this.filteredOptions = this.searchControl.valueChanges.pipe(
      startWith(''),
      debounceTime(300),
      distinctUntilChanged(),
      map(value => this.filterEmployees(value || ''))
    );
  }

  openAddEditEmployeeDialog() {
    const dialogRef = this.dialog.open(EmployeeAddEditDialogComponent);
    dialogRef.afterClosed().subscribe(val => {
      if (val) {
        this.getEmployeeList();
      }
    });
  }

  getEmployeeList() {
    this.employeeService.getEmployees().subscribe({
      next: (res) => {
        this.dataSource = new MatTableDataSource(res);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
      },
      error: (err) => console.error(err),
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value.trim().toLowerCase();
    this.dataSource.filter = filterValue;
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  filterEmployees(value: string): string[] {
    const filterValue = value.toLowerCase();
    return this.dataSource.data
      .map(emp => `${emp.firstName} ${emp.lastName}`)
      .filter(option => option.toLowerCase().includes(filterValue));
  }

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

  openEditForm(data: any) {
    const dialogRef = this.dialog.open(EmployeeAddEditDialogComponent, { data });
    dialogRef.afterClosed().subscribe(val => {
      if (val) {
        this.getEmployeeList();
      }
    });
  }
}
