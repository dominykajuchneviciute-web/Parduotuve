import type { ItemDto, ItemFilterState } from "../types";

const conditionNames = [
  "New",
  "VeryGood",
  "Good",
  "Decent",
  "Used",
  "VeryUsed",
];

const matchesText = (value: string | null | undefined, filter: string) =>
  !filter || (value?.toLowerCase().includes(filter.toLowerCase()) ?? false);

const matchesNumber = (
  value: number | null | undefined,
  filter: string,
) => !filter || value === Number(filter);

export function matchesItemFilters(
  item: ItemDto,
  filters: ItemFilterState,
): boolean {
  const matchesCondition =
    !filters.selectedCondition ||
    item.condition?.toString() === filters.selectedCondition ||
    item.condition?.toString() ===
      conditionNames[Number(filters.selectedCondition)];

  if (
    !matchesText(item.name, filters.searchQuery) ||
    !matchesCondition ||
    (filters.selectedCategory &&
      item.itemType !== filters.selectedCategory)
  ) {
    return false;
  }

  switch (filters.selectedCategory) {
    case "Clothing":
      return (
        matchesNumber(item.size, filters.size) &&
        matchesText(item.color, filters.color) &&
        matchesText(item.manufacturer, filters.manufacturer)
      );
    case "Electronics":
      return (
        matchesText(item.manufacturer, filters.manufacturer) &&
        matchesText(item.model, filters.model) &&
        matchesText(item.color, filters.color)
      );
    case "Furniture":
      return (
        matchesText(item.material, filters.material) &&
        matchesText(item.color, filters.color) &&
        matchesText(item.dimensions, filters.dimensions)
      );
    case "Transport":
      return (
        matchesText(item.manufacturer, filters.manufacturer) &&
        matchesText(item.model, filters.model) &&
        matchesNumber(item.year, filters.year)
      );
    case "Footwear":
      return (
        matchesNumber(item.shoeSize, filters.shoeSize) &&
        matchesText(item.color, filters.color) &&
        matchesText(item.manufacturer, filters.manufacturer)
      );
    case "Instrument":
      return (
        matchesText(item.instrumentType, filters.instrumentType) &&
        matchesText(item.manufacturer, filters.manufacturer) &&
        matchesNumber(item.year, filters.year)
      );
    default:
      return true;
  }
}
