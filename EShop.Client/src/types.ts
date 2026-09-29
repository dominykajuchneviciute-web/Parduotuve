export interface ItemDto {
  id: number;
  name: string;
  description?: string;
  condition?: number;
  size?: number;
  manufacturer?: string;
  color?: string;
  // Laikinai uzkomentavau kol nera backende
  // category?: string;
  // exchangeType?: string;
  // lookingFor?: string;
}