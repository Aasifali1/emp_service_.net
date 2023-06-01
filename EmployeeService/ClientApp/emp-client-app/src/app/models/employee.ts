export class Employee {
  id: number;
  name: string;
  email: string;
  cityId: number;

  constructor(id: number,
    name: string,
    email: string,
    cityId: number,
  ) {
    this.id = id;
    this.name = name;
    this.email = email;
    this.cityId = cityId;
  }
}
