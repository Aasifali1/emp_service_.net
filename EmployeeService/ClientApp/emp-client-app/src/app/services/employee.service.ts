import { Injectable } from '@angular/core';
import { Employee } from '../models/employee';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  employees: Employee[] = [];
  
  constructor(private httpClient: HttpClient) {
   
  }

  public getEmployees(): Observable<Employee[]> {
    let headers = new HttpHeaders();
    headers = headers.append("Authorization", "Bearer mytoken");
    return this.httpClient.get<Employee[]>("https://localhost:7042/api/employee",
      { headers : headers });
  }
}
