export async function httpPost<TData, TReturn>(
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

export async function httpPostFiles(
  url: string,
  data?: FormData
): Promise<void> {
  await fetch(url, {
    method: 'POST',
    credentials: 'include',
    body: data,
  });
}

export async function httpGet<TReturn>(url: string): Promise<TReturn> {
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

export async function httpDelete(url: string): Promise<void> {
  await fetch(url, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
    },
    credentials: 'include',
  });
}
