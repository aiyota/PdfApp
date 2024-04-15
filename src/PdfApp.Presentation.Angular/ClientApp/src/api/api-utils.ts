export async function post<TData, TReturn>(
  url: string,
  data?: TData
): Promise<TReturn> {
  const response = await fetch(url, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    credentials: 'include',
    body: JSON.stringify(data),
  });

  const responseData = await response.json();

  return responseData as TReturn;
}

export async function get<TReturn>(url: string): Promise<TReturn> {
  const response = await fetch(url, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
    credentials: 'include',
  });

  const responseData = await response.json();

  return responseData as TReturn;
}
