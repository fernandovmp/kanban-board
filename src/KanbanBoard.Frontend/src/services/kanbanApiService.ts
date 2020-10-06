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
    data?: T | IErrorResponse;
}

interface IApiFetchParams extends IApiActionParams {
    method?: 'GET' | 'POST' | 'PATCH' | 'PUT' | 'DELETE';
    contentType?: 'application/json' | 'application/merge-patch+json';
}

function apiFetch(params: IApiFetchParams) {
    const { uri, method, body, bearerToken, contentType } = params;
    const url = `${baseUrl}/api/${uri}`;
    const authorizationHeader = bearerToken
        ? [['authorization', `Bearer ${bearerToken}`]]
        : [];
    return fetch(url, {
        method: method,
        headers: [
            ['content-type', contentType ?? 'application/json'],
            ...authorizationHeader,
        ],
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

export async function apiGet<TResponse = any>(params: IApiActionParams) {
    return _apiAction<TResponse>({ ...params, method: 'GET' });
}

export async function apiPost<TResponse = any>(params: IApiActionParams) {
    return _apiAction<TResponse>({ ...params, method: 'POST' });
}

export async function apiPatch<TResponse = any>(params: IApiActionParams) {
    return _apiAction<TResponse>({
        ...params,
        method: 'PATCH',
        contentType: 'application/merge-patch+json',
    });
}

export async function apiPut<TResponse = any>(params: IApiActionParams) {
    return _apiAction<TResponse>({ ...params, method: 'PUT' });
}

export async function apiDelete<TResponse = any>(params: IApiActionParams) {
    return _apiAction<TResponse>({ ...params, method: 'DELETE' });
}

async function _apiAction<TResponse = any>(
    params: IApiFetchParams
): Promise<IApiResponse<TResponse>> {
    const response = await apiFetch(params);
    if (response.status === 401) {
        return unauthorizedResponse();
    }
    if (response.status === 403) {
        return { data: { status: 403, message: 'Forbidden' } };
    }
    if (response.status === 204) {
        return { data: undefined };
    }
    const responseData = await response.json();
    return { data: responseData };
}

export function isErrorResponse(
    responseData: IErrorResponse | any
): responseData is IErrorResponse {
    return (responseData as IErrorResponse).status !== undefined;
}
