/**
 * Creates a debounce function that will call the provided function after the specified delay.
 */
export function makeDebouncer<TReturn>(
  delay: number
): (fn: () => Promise<TReturn>) => Promise<TReturn> {
  let timeoutId: number;

  return (fn: () => Promise<TReturn>) =>
    new Promise<TReturn>((resolve) => {
      window.clearTimeout(timeoutId);
      timeoutId = window.setTimeout(() => resolve(fn()), delay);
    });
}
