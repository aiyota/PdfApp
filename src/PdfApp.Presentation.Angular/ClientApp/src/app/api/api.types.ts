export type User = {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
};

/**
 * The user object that is generated from the token.
 */
export type ClaimsUser = {
  id: string;
  email: string;
  userName: string;
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
  hasFile: boolean;
};

export type Tag = {
  id: number;
  name: string;
};

export type TagRequest = {
  name: string;
};

export type PdfUploadRequest = {
  title: string;
  description: string;
  author: string;
  totalPages: number;
  tags: TagRequest[];
};

export type Progress = {
  id: number;
  name: string;
  page: number;
};
