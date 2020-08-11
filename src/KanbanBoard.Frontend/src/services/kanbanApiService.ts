const baseUrl = process.env.REACT_APP_KANBAN_API_URL;

interface IApiActionParams {
    uri?: string;
    body?: any;
    bearerToken?: string;
}

export interface IApiValidationError {
    property: string;
    message: string;
}

export interface IErrorResponse {
    status: number;
    message: string;
    errors?: IApiValidationError[];
}

export interface IApiResponse<T> {
    data: T | IErrorResponse;
}

interface IApiFetchParams extends IApiActionParams {
    method?: 'GET' | 'POST' | 'PATCH' | 'PUT' | 'DELETE';
}

function apiFetch(params: IApiFetchParams) {
    const { uri, method, body, bearerToken } = params;
    const url = `${baseUrl}/api/${uri}`;
    const authorizationHeader = bearerToken
        ? [['authorization', `Bearer ${bearerToken}`]]
        : [];
    return fetch(url, {
        method: method,
        headers: [['content-type', 'application/json'], ...authorizationHeader],
        body: body ? JSON.stringify(body) : undefined,
    });
}

function unauthorizedResponse() {
    return {
        data: {
            message: 'Unauthorized',
            status: 401,
        },
    };
}

export async function apiPost<TResponse = any>(params: IApiActionParams) {
    return await _apiAction<TResponse>({ ...params, method: 'POST' });
}

async function _apiAction<TResponse = any>(
    params: IApiFetchParams
): Promise<IApiResponse<TResponse>> {
    const response = await apiFetch(params);
    if (response.status === 401) {
        return unauthorizedResponse();
    }
    const responseData = await response.json();
    return { data: responseData };
}

export function isErrorResponse(
    responseData: IErrorResponse | any
): responseData is IErrorResponse {
    return (responseData as IErrorResponse).status !== undefined;
}
