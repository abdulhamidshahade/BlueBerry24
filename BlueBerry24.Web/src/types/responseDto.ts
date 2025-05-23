export interface ResponseDto<T> {
  isSuccess: boolean;
  statusCode: number;
  statusMessage: string;
  data: T;
  errors?: string[];
}