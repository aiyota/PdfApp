export type User = {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
};

export type LoginRequest = {
  email: string;
  password: string;
};

export type Pdf = {
  id: number;
  title: string;
  description: string;
  author: string;
  totalPages: number;
  fileName: string;
  tags: Tag[];
  createdOn: string;
  lastAccessed: string;
};

export type Tag = {
  id: number;
  name: string;
};
