import { Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EmployeeService } from '../../shared/services/employee.service';
import { Employee } from '../../shared/models/employee';
import { dateRangeValidator, exactLengthValidator } from '../../shared/utils/custom-validators';


@Component({
  selector: 'app-employee-add-edit-dialog',
  templateUrl: './employee-add-edit-dialog.component.html',
  styleUrls: ['./employee-add-edit-dialog.component.css'],
  standalone: false
})
export class EmployeeAddEditDialogComponent implements OnInit {
  employeeForm!: FormGroup;
  isLoading = false; 

  constructor(
    private formBuilder: FormBuilder,
    private employeeService: EmployeeService,
    private dialogRef: MatDialogRef<EmployeeAddEditDialogComponent>,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: Employee
  ) { }

  ngOnInit(): void {
    this.employeeForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      birthDate: ['', Validators.required],
      employeeNumber: ['', [Validators.required, exactLengthValidator(16)]],
      employedDate: ['', Validators.required],
      terminatedDate: [null, dateRangeValidator('employedDate')],

    });

    if (this.data) {
      this.employeeForm.patchValue(this.data);
    }
  }

  onSubmit(): void {
    if (this.employeeForm.invalid) {
      this.employeeForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const employee: Employee = {
      employeeId: this.data ? this.data.employeeId : undefined,
      ...this.employeeForm.value,
    };

    const apiCall = this.data
      ? this.employeeService.updateEmployee(employee.employeeId!, employee)
      : this.employeeService.addEmployee(employee);

    apiCall.subscribe({
      next: () => {
        this.snackBar.open(
          `Employee ${this.data ? 'updated' : 'added'} successfully!`,
          'Close',
          { duration: 3000 }
        );
        this.dialogRef.close(true);
      },
      error: (err) => {
        console.error(err);
        this.snackBar.open(
          'An error occurred. Please try again.',
          'Close',
          { duration: 3000 }
        );
        this.isLoading = false;
      },
      complete: () => {
        this.isLoading = false;
      },
    });
  }
}
