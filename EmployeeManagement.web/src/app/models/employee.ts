export interface Employee {
  employeeId: number;
  name: string;
  email: string;
  salary: number;
  departmentId: number;
  departmentName?: string;
  createdDate?: string;
}
