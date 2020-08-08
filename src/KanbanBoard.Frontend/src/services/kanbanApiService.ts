import { User } from '../models';

const baseUrl = process.env.REACT_APP_KANBAN_API_URL;

interface IApiActionParams {
    uri?: string;
    body?: any;
}

interface IApiFetchParams extends IApiActionParams {
    method?: 'GET' | 'POST' | 'PATCH' | 'PUT' | 'DELETE';
}

function apiFetch(params: IApiFetchParams) {
    const { uri, method, body } = params;
    const url = `${baseUrl}/api/${uri}`;
    return fetch(url, {
        method: method,
        headers: [['content-type', 'application/json']],
        body: body ? JSON.stringify(body) : undefined,
    });
}

function apiPost(params: IApiActionParams) {
    const { uri, body } = params;
    return apiFetch({ method: 'POST', uri, body });
}

export interface IApiValidationError {
    property: string;
    message: string;
}

interface IErrorResponse {
    status: number;
    message: string;
    errors?: IApiValidationError[];
}

export interface IResponse<T> {
    data: T | IErrorResponse;
}

type LoginData = {
    email: string;
    password: string;
};

type LoginResponse = {
    token: string;
    user: User;
};

export async function fetchLogin(
    data: LoginData
): Promise<IResponse<LoginResponse>> {
    const response = await apiPost({ uri: 'v1/login', body: data });
    const responseData = await response.json();
    return { data: responseData };
}

type SignUpData = {
    name: string;
    email: string;
    password: string;
    confirmPassword: string;
};

type SignUpResponse = {
    id: number;
    name: string;
    email: string;
};

export async function fetchSignUp(
    data: SignUpData
): Promise<IResponse<SignUpResponse>> {
    const result = await apiPost({ uri: 'v1/users', body: data });
    const responseData = await result.json();
    return { data: responseData };
}

export function isErrorResponse(
    responseData: IErrorResponse | any
): responseData is IErrorResponse {
    return (responseData as IErrorResponse).status !== undefined;
}
