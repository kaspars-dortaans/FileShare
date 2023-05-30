import type { Size } from "../../common/size";

export interface FileFormData {
  id: number | null;
  name: string | null;
  comment: string | null;
  extension: string | null;
  minSize: Size | null;
  maxFileSize: number | null;
  allowedExtensions: string | null;
}