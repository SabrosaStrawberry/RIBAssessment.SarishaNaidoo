export interface Employee {
  employeeId?: number;
  firstName: string;
  lastName: string;
  birthDate: Date;
  employeeNumber: string;
  employedDate: Date;
  terminatedDate?: Date | null;
}