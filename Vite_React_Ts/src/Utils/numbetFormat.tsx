export const NumberFormat = (value: number): string => {
    return Intl.NumberFormat("de-DE", {
      style: "currency",
      currency: "USD",
    }).format(value);
  };
