export const formatText = (text: string, quantity = 30) =>
  text.length > quantity ? `${text.substring(0, quantity)}...` : text;
