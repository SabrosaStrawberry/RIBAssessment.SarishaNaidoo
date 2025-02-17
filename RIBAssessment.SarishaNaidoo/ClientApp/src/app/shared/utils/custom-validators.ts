import { ValidatorFn, AbstractControl, ValidationErrors } from "@angular/forms";

export function dateRangeValidator(ControlName: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const firstDate = control.parent?.get(ControlName)?.value;
      const lastDate = control.value;
  
      if (firstDate && lastDate && new Date(lastDate) < new Date(firstDate)) {
        return { dateRange: true }; 
      }
      return null; 
    }
}

export function exactLengthValidator(length: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value: string = control.value || '';
      if (value.length !== length) {
        return {exactLength: true, requiredLength: length}
        };
      
      return null;
    };
  }