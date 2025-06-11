export interface User {
    id: number;
    email: string;
    userName: string;
    firstName?: string;
    lastName?: string;
    roles: string[]; 
}

export interface UpdateUserData {
  email: string;
  userName: string;
  firstName?: string;
  lastName?: string;
  emailConfirmed: boolean;
}

export interface CreateUserData {
  email: string;
  userName: string;
  firstName?: string;
  lastName?: string;
  password: string;
  emailConfirmed?: boolean;
  roles?: string[];
} 