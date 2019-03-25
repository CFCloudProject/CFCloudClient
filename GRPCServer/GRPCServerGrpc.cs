// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: GRPCServer.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace GRPCServer {
  public static partial class GRPCServer
  {
    static readonly string __ServiceName = "GRPCServer.GRPCServer";

    static readonly grpc::Marshaller<global::GRPCServer.User> __Marshaller_GRPCServer_User = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GRPCServer.User.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::GRPCServer.RegisterResult> __Marshaller_GRPCServer_RegisterResult = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GRPCServer.RegisterResult.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::GRPCServer.LoginResult> __Marshaller_GRPCServer_LoginResult = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GRPCServer.LoginResult.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::GRPCServer.EmptyRequest> __Marshaller_GRPCServer_EmptyRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GRPCServer.EmptyRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::GRPCServer.StringResponse> __Marshaller_GRPCServer_StringResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GRPCServer.StringResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::GRPCServer.ShareRequest> __Marshaller_GRPCServer_ShareRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GRPCServer.ShareRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::GRPCServer.PathRequest> __Marshaller_GRPCServer_PathRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GRPCServer.PathRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::GRPCServer.RenameRequest> __Marshaller_GRPCServer_RenameRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GRPCServer.RenameRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::GRPCServer.UploadRequest> __Marshaller_GRPCServer_UploadRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GRPCServer.UploadRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::GRPCServer.BlockRequest> __Marshaller_GRPCServer_BlockRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GRPCServer.BlockRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::GRPCServer.BlockResponse> __Marshaller_GRPCServer_BlockResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GRPCServer.BlockResponse.Parser.ParseFrom);

    static readonly grpc::Method<global::GRPCServer.User, global::GRPCServer.RegisterResult> __Method_Register = new grpc::Method<global::GRPCServer.User, global::GRPCServer.RegisterResult>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Register",
        __Marshaller_GRPCServer_User,
        __Marshaller_GRPCServer_RegisterResult);

    static readonly grpc::Method<global::GRPCServer.User, global::GRPCServer.LoginResult> __Method_Login = new grpc::Method<global::GRPCServer.User, global::GRPCServer.LoginResult>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Login",
        __Marshaller_GRPCServer_User,
        __Marshaller_GRPCServer_LoginResult);

    static readonly grpc::Method<global::GRPCServer.EmptyRequest, global::GRPCServer.StringResponse> __Method_Logout = new grpc::Method<global::GRPCServer.EmptyRequest, global::GRPCServer.StringResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Logout",
        __Marshaller_GRPCServer_EmptyRequest,
        __Marshaller_GRPCServer_StringResponse);

    static readonly grpc::Method<global::GRPCServer.EmptyRequest, global::GRPCServer.StringResponse> __Method_HeartBeat = new grpc::Method<global::GRPCServer.EmptyRequest, global::GRPCServer.StringResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "HeartBeat",
        __Marshaller_GRPCServer_EmptyRequest,
        __Marshaller_GRPCServer_StringResponse);

    static readonly grpc::Method<global::GRPCServer.ShareRequest, global::GRPCServer.StringResponse> __Method_Share = new grpc::Method<global::GRPCServer.ShareRequest, global::GRPCServer.StringResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Share",
        __Marshaller_GRPCServer_ShareRequest,
        __Marshaller_GRPCServer_StringResponse);

    static readonly grpc::Method<global::GRPCServer.PathRequest, global::GRPCServer.StringResponse> __Method_CreateFolder = new grpc::Method<global::GRPCServer.PathRequest, global::GRPCServer.StringResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "CreateFolder",
        __Marshaller_GRPCServer_PathRequest,
        __Marshaller_GRPCServer_StringResponse);

    static readonly grpc::Method<global::GRPCServer.RenameRequest, global::GRPCServer.StringResponse> __Method_Rename = new grpc::Method<global::GRPCServer.RenameRequest, global::GRPCServer.StringResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Rename",
        __Marshaller_GRPCServer_RenameRequest,
        __Marshaller_GRPCServer_StringResponse);

    static readonly grpc::Method<global::GRPCServer.PathRequest, global::GRPCServer.StringResponse> __Method_Delete = new grpc::Method<global::GRPCServer.PathRequest, global::GRPCServer.StringResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Delete",
        __Marshaller_GRPCServer_PathRequest,
        __Marshaller_GRPCServer_StringResponse);

    static readonly grpc::Method<global::GRPCServer.UploadRequest, global::GRPCServer.StringResponse> __Method_Upload = new grpc::Method<global::GRPCServer.UploadRequest, global::GRPCServer.StringResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Upload",
        __Marshaller_GRPCServer_UploadRequest,
        __Marshaller_GRPCServer_StringResponse);

    static readonly grpc::Method<global::GRPCServer.BlockRequest, global::GRPCServer.StringResponse> __Method_UploadBlock = new grpc::Method<global::GRPCServer.BlockRequest, global::GRPCServer.StringResponse>(
        grpc::MethodType.ClientStreaming,
        __ServiceName,
        "UploadBlock",
        __Marshaller_GRPCServer_BlockRequest,
        __Marshaller_GRPCServer_StringResponse);

    static readonly grpc::Method<global::GRPCServer.PathRequest, global::GRPCServer.StringResponse> __Method_Download = new grpc::Method<global::GRPCServer.PathRequest, global::GRPCServer.StringResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Download",
        __Marshaller_GRPCServer_PathRequest,
        __Marshaller_GRPCServer_StringResponse);

    static readonly grpc::Method<global::GRPCServer.BlockRequest, global::GRPCServer.BlockResponse> __Method_DownloadBlock = new grpc::Method<global::GRPCServer.BlockRequest, global::GRPCServer.BlockResponse>(
        grpc::MethodType.DuplexStreaming,
        __ServiceName,
        "DownloadBlock",
        __Marshaller_GRPCServer_BlockRequest,
        __Marshaller_GRPCServer_BlockResponse);

    static readonly grpc::Method<global::GRPCServer.PathRequest, global::GRPCServer.StringResponse> __Method_GetMetadata = new grpc::Method<global::GRPCServer.PathRequest, global::GRPCServer.StringResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetMetadata",
        __Marshaller_GRPCServer_PathRequest,
        __Marshaller_GRPCServer_StringResponse);

    static readonly grpc::Method<global::GRPCServer.PathRequest, global::GRPCServer.StringResponse> __Method_ListFolder = new grpc::Method<global::GRPCServer.PathRequest, global::GRPCServer.StringResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ListFolder",
        __Marshaller_GRPCServer_PathRequest,
        __Marshaller_GRPCServer_StringResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::GRPCServer.GRPCServerReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of GRPCServer</summary>
    public abstract partial class GRPCServerBase
    {
      public virtual global::System.Threading.Tasks.Task<global::GRPCServer.RegisterResult> Register(global::GRPCServer.User request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::GRPCServer.LoginResult> Login(global::GRPCServer.User request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::GRPCServer.StringResponse> Logout(global::GRPCServer.EmptyRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::GRPCServer.StringResponse> HeartBeat(global::GRPCServer.EmptyRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::GRPCServer.StringResponse> Share(global::GRPCServer.ShareRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::GRPCServer.StringResponse> CreateFolder(global::GRPCServer.PathRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::GRPCServer.StringResponse> Rename(global::GRPCServer.RenameRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::GRPCServer.StringResponse> Delete(global::GRPCServer.PathRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::GRPCServer.StringResponse> Upload(global::GRPCServer.UploadRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::GRPCServer.StringResponse> UploadBlock(grpc::IAsyncStreamReader<global::GRPCServer.BlockRequest> requestStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::GRPCServer.StringResponse> Download(global::GRPCServer.PathRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task DownloadBlock(grpc::IAsyncStreamReader<global::GRPCServer.BlockRequest> requestStream, grpc::IServerStreamWriter<global::GRPCServer.BlockResponse> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::GRPCServer.StringResponse> GetMetadata(global::GRPCServer.PathRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::GRPCServer.StringResponse> ListFolder(global::GRPCServer.PathRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for GRPCServer</summary>
    public partial class GRPCServerClient : grpc::ClientBase<GRPCServerClient>
    {
      /// <summary>Creates a new client for GRPCServer</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public GRPCServerClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for GRPCServer that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public GRPCServerClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected GRPCServerClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected GRPCServerClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::GRPCServer.RegisterResult Register(global::GRPCServer.User request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Register(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::GRPCServer.RegisterResult Register(global::GRPCServer.User request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Register, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.RegisterResult> RegisterAsync(global::GRPCServer.User request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return RegisterAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.RegisterResult> RegisterAsync(global::GRPCServer.User request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Register, null, options, request);
      }
      public virtual global::GRPCServer.LoginResult Login(global::GRPCServer.User request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Login(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::GRPCServer.LoginResult Login(global::GRPCServer.User request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Login, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.LoginResult> LoginAsync(global::GRPCServer.User request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return LoginAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.LoginResult> LoginAsync(global::GRPCServer.User request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Login, null, options, request);
      }
      public virtual global::GRPCServer.StringResponse Logout(global::GRPCServer.EmptyRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Logout(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::GRPCServer.StringResponse Logout(global::GRPCServer.EmptyRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Logout, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> LogoutAsync(global::GRPCServer.EmptyRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return LogoutAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> LogoutAsync(global::GRPCServer.EmptyRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Logout, null, options, request);
      }
      public virtual global::GRPCServer.StringResponse HeartBeat(global::GRPCServer.EmptyRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return HeartBeat(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::GRPCServer.StringResponse HeartBeat(global::GRPCServer.EmptyRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_HeartBeat, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> HeartBeatAsync(global::GRPCServer.EmptyRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return HeartBeatAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> HeartBeatAsync(global::GRPCServer.EmptyRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_HeartBeat, null, options, request);
      }
      public virtual global::GRPCServer.StringResponse Share(global::GRPCServer.ShareRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Share(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::GRPCServer.StringResponse Share(global::GRPCServer.ShareRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Share, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> ShareAsync(global::GRPCServer.ShareRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ShareAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> ShareAsync(global::GRPCServer.ShareRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Share, null, options, request);
      }
      public virtual global::GRPCServer.StringResponse CreateFolder(global::GRPCServer.PathRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CreateFolder(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::GRPCServer.StringResponse CreateFolder(global::GRPCServer.PathRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_CreateFolder, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> CreateFolderAsync(global::GRPCServer.PathRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CreateFolderAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> CreateFolderAsync(global::GRPCServer.PathRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_CreateFolder, null, options, request);
      }
      public virtual global::GRPCServer.StringResponse Rename(global::GRPCServer.RenameRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Rename(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::GRPCServer.StringResponse Rename(global::GRPCServer.RenameRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Rename, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> RenameAsync(global::GRPCServer.RenameRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return RenameAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> RenameAsync(global::GRPCServer.RenameRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Rename, null, options, request);
      }
      public virtual global::GRPCServer.StringResponse Delete(global::GRPCServer.PathRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Delete(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::GRPCServer.StringResponse Delete(global::GRPCServer.PathRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Delete, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> DeleteAsync(global::GRPCServer.PathRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return DeleteAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> DeleteAsync(global::GRPCServer.PathRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Delete, null, options, request);
      }
      public virtual global::GRPCServer.StringResponse Upload(global::GRPCServer.UploadRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Upload(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::GRPCServer.StringResponse Upload(global::GRPCServer.UploadRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Upload, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> UploadAsync(global::GRPCServer.UploadRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return UploadAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> UploadAsync(global::GRPCServer.UploadRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Upload, null, options, request);
      }
      public virtual grpc::AsyncClientStreamingCall<global::GRPCServer.BlockRequest, global::GRPCServer.StringResponse> UploadBlock(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return UploadBlock(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncClientStreamingCall<global::GRPCServer.BlockRequest, global::GRPCServer.StringResponse> UploadBlock(grpc::CallOptions options)
      {
        return CallInvoker.AsyncClientStreamingCall(__Method_UploadBlock, null, options);
      }
      public virtual global::GRPCServer.StringResponse Download(global::GRPCServer.PathRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Download(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::GRPCServer.StringResponse Download(global::GRPCServer.PathRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Download, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> DownloadAsync(global::GRPCServer.PathRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return DownloadAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> DownloadAsync(global::GRPCServer.PathRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Download, null, options, request);
      }
      public virtual grpc::AsyncDuplexStreamingCall<global::GRPCServer.BlockRequest, global::GRPCServer.BlockResponse> DownloadBlock(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return DownloadBlock(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncDuplexStreamingCall<global::GRPCServer.BlockRequest, global::GRPCServer.BlockResponse> DownloadBlock(grpc::CallOptions options)
      {
        return CallInvoker.AsyncDuplexStreamingCall(__Method_DownloadBlock, null, options);
      }
      public virtual global::GRPCServer.StringResponse GetMetadata(global::GRPCServer.PathRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetMetadata(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::GRPCServer.StringResponse GetMetadata(global::GRPCServer.PathRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetMetadata, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> GetMetadataAsync(global::GRPCServer.PathRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetMetadataAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> GetMetadataAsync(global::GRPCServer.PathRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetMetadata, null, options, request);
      }
      public virtual global::GRPCServer.StringResponse ListFolder(global::GRPCServer.PathRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ListFolder(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::GRPCServer.StringResponse ListFolder(global::GRPCServer.PathRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ListFolder, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> ListFolderAsync(global::GRPCServer.PathRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ListFolderAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::GRPCServer.StringResponse> ListFolderAsync(global::GRPCServer.PathRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ListFolder, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override GRPCServerClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new GRPCServerClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(GRPCServerBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Register, serviceImpl.Register)
          .AddMethod(__Method_Login, serviceImpl.Login)
          .AddMethod(__Method_Logout, serviceImpl.Logout)
          .AddMethod(__Method_HeartBeat, serviceImpl.HeartBeat)
          .AddMethod(__Method_Share, serviceImpl.Share)
          .AddMethod(__Method_CreateFolder, serviceImpl.CreateFolder)
          .AddMethod(__Method_Rename, serviceImpl.Rename)
          .AddMethod(__Method_Delete, serviceImpl.Delete)
          .AddMethod(__Method_Upload, serviceImpl.Upload)
          .AddMethod(__Method_UploadBlock, serviceImpl.UploadBlock)
          .AddMethod(__Method_Download, serviceImpl.Download)
          .AddMethod(__Method_DownloadBlock, serviceImpl.DownloadBlock)
          .AddMethod(__Method_GetMetadata, serviceImpl.GetMetadata)
          .AddMethod(__Method_ListFolder, serviceImpl.ListFolder).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, GRPCServerBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_Register, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GRPCServer.User, global::GRPCServer.RegisterResult>(serviceImpl.Register));
      serviceBinder.AddMethod(__Method_Login, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GRPCServer.User, global::GRPCServer.LoginResult>(serviceImpl.Login));
      serviceBinder.AddMethod(__Method_Logout, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GRPCServer.EmptyRequest, global::GRPCServer.StringResponse>(serviceImpl.Logout));
      serviceBinder.AddMethod(__Method_HeartBeat, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GRPCServer.EmptyRequest, global::GRPCServer.StringResponse>(serviceImpl.HeartBeat));
      serviceBinder.AddMethod(__Method_Share, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GRPCServer.ShareRequest, global::GRPCServer.StringResponse>(serviceImpl.Share));
      serviceBinder.AddMethod(__Method_CreateFolder, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GRPCServer.PathRequest, global::GRPCServer.StringResponse>(serviceImpl.CreateFolder));
      serviceBinder.AddMethod(__Method_Rename, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GRPCServer.RenameRequest, global::GRPCServer.StringResponse>(serviceImpl.Rename));
      serviceBinder.AddMethod(__Method_Delete, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GRPCServer.PathRequest, global::GRPCServer.StringResponse>(serviceImpl.Delete));
      serviceBinder.AddMethod(__Method_Upload, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GRPCServer.UploadRequest, global::GRPCServer.StringResponse>(serviceImpl.Upload));
      serviceBinder.AddMethod(__Method_UploadBlock, serviceImpl == null ? null : new grpc::ClientStreamingServerMethod<global::GRPCServer.BlockRequest, global::GRPCServer.StringResponse>(serviceImpl.UploadBlock));
      serviceBinder.AddMethod(__Method_Download, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GRPCServer.PathRequest, global::GRPCServer.StringResponse>(serviceImpl.Download));
      serviceBinder.AddMethod(__Method_DownloadBlock, serviceImpl == null ? null : new grpc::DuplexStreamingServerMethod<global::GRPCServer.BlockRequest, global::GRPCServer.BlockResponse>(serviceImpl.DownloadBlock));
      serviceBinder.AddMethod(__Method_GetMetadata, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GRPCServer.PathRequest, global::GRPCServer.StringResponse>(serviceImpl.GetMetadata));
      serviceBinder.AddMethod(__Method_ListFolder, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GRPCServer.PathRequest, global::GRPCServer.StringResponse>(serviceImpl.ListFolder));
    }

  }
}
#endregion
