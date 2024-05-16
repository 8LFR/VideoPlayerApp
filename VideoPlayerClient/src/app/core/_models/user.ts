export interface User {
  id: string;
  name: string;
  created: string;
  lastActive: string;
  avatarUrl: string;
}

export interface LoginUser {
  name: string;
  password: string;
}

export interface RegisterUser {
  name: string;
  password: string;
}
