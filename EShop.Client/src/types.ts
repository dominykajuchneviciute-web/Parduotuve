export interface ItemDto {
  id: number;
  name: string;
  description: string;
  condition?: string;
  size?: string;
  manufacturer?: string;
  color?: string;
  // Laikinai uzkomentavau kol nera backende
  // category?: string;
  // exchangeType?: string;
  // lookingFor?: string;
}