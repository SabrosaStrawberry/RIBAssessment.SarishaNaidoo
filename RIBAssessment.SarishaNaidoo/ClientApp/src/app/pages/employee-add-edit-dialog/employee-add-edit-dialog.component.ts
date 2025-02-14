import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EmployeeService } from '../../shared/services/employee.service';

@Component({
  selector: 'app-employee-add-edit-dialog',
  templateUrl: './employee-add-edit-dialog.component.html',
  styleUrls: ['./employee-add-edit-dialog.component.css'],
  standalone: false
})
export class EmployeeAddEditDialogComponent implements OnInit {
  employeeForm!: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private employeeService: EmployeeService,
    private dialogRef: MatDialogRef<EmployeeAddEditDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  ngOnInit(): void {
    this.employeeForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      birthDate: ['', Validators.required],
      employeeNumber: ['', Validators.required],
      employedDate: ['', Validators.required],
      terminatedDate: [null]
    });

    if (this.data) {
      this.employeeForm.patchValue(this.data);
    }
  }

  onSubmit(): void {
    if (this.employeeForm.valid) {
      const employee = {
        employeeId: this.data ? this.data.employeeId : 0,
        ...this.employeeForm.value
      };

      if (this.data) {
        this.employeeService.updateEmployee(this.data.employeeId,employee).subscribe({
          next: () => {
            alert('Employee updated successfully');
            this.dialogRef.close(true);
          },
          error: (err) => console.error(err)
        });
      } else {
        this.employeeService.addEmployee(employee).subscribe({
          next: () => {
            alert('Employee added successfully');
            this.dialogRef.close(true);
          },
          error: (err) => console.error(err)
        });
      }
    }
  }
}
