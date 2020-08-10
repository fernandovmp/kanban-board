import { User } from '../models';

const baseUrl = process.env.REACT_APP_KANBAN_API_URL;

interface IApiActionParams {
    uri?: string;
    body?: any;
    bearerToken?: string;
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

function apiPost(params: IApiActionParams) {
    const { uri, body, bearerToken } = params;
    return apiFetch({ method: 'POST', uri, body, bearerToken });
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

type PostBoardData = {
    title: string;
};

type PostBoardResponse = {
    id: number;
    title: string;
    createOn: Date;
    modifiedOn: Date;
};

export async function postBoard(
    data: PostBoardData,
    token: string
): Promise<IResponse<PostBoardResponse>> {
    const result = await apiPost({
        uri: 'v1/boards',
        body: data,
        bearerToken: token,
    });
    const responseData = await result.json();
    return { data: responseData };
}

export function isErrorResponse(
    responseData: IErrorResponse | any
): responseData is IErrorResponse {
    return (responseData as IErrorResponse).status !== undefined;
}
