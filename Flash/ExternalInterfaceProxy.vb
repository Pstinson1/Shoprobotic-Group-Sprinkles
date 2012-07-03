'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************



Imports System
Imports System.Runtime.InteropServices
Imports AxShockwaveFlashObjects

Namespace Flash.External
    ' <summary> 
    ' Facilitates External Interface communication between a .NET application and a Shockwave 
    ' Flash ActiveX control by providing an abstraction layer over the XML-serialized data format 
    ' used by Flash Player for ExternalInterface communication. 
    ' This class provides the Call method for calling ActionScript functions and raises 
    ' the ExternalInterfaceCall event when calls come from ActionScript. 
    ' </summary> 
    Public Class ExternalInterfaceProxy

#Region "Private Fields"

        Private _flashControl As AxShockwaveFlash


#End Region

#Region "Constructor"

        ' <summary> 
        ' Creates a new ExternalInterfaceProxy for the specified Shockwave Flash ActiveX control. 
        ' </summary> 
        ' <param name="flashControl">The Shockwave Flash ActiveX control with whom communication 
        ' is managed by this proxy.</param> 
        Public Sub New(ByRef flashControl As AxShockwaveFlash)
            _flashControl = flashControl
            AddHandler _flashControl.FlashCall, AddressOf _flashControl_FlashCall
            AddHandler _flashControl.FSCommand, AddressOf _flashControl_FSCommandCall
        End Sub

#End Region

#Region "Public Methods"

        ' <summary> 
        ' Calls the ActionScript function which is registered as a callback method with the 
        ' ActionScript ExternalInterface class. 
        ' </summary> 
        ' <param name="functionName">The function name registered with the ExternalInterface class 
        ' corresponding to the ActionScript function that is to be called</param> 
        ' <param name="arguments">Additional arguments, if any, to pass to the ActionScript function.</param> 
        ' <returns>The result returned by the ActionScript function, or null if no result is returned.</returns> 
        ' <exception cref="System.Runtime.InteropServices.COMException">Thrown when there is an error 
        ' calling the method on Flash Player. For instance, this exception is raised if the 
        ' specified function name is not registered as a callable function with the ExternalInterface 
        ' class; it is also raised if the ActionScript method throws an Error.</exception> 

        Public Delegate Function CallMeDelegate(ByVal functionName As String, ByVal arguments As Object()) As Object

        Public Function CallMe(ByVal functionName As String, ByVal ParamArray arguments As Object()) As Object
            Try
                Dim request As String = ExternalInterfaceSerializer.EncodeInvoke(functionName, arguments)
                Dim response As String = _flashControl.CallFunction(request)
                Dim result As Object = ExternalInterfaceSerializer.DecodeResult(response)
                Return result
            Catch generatedExceptionName As Exception
                Debug.WriteLine(generatedExceptionName.ToString)
                Throw
            End Try
        End Function

#End Region

#Region "Events"

        ' <summary> 
        ' Raised when an External Interface call is made from Flash Player. 
        ' </summary> 
        Public Event ExternalInterfaceCall As ExternalInterfaceCallEventHandler
        Public Event ExternalFSCommandCall As AxShockwaveFlashObjects._IShockwaveFlashEvents_FSCommandEventHandler

        Protected Overridable Function OnExternalFSCommandCall(ByVal e As _IShockwaveFlashEvents_FSCommandEvent) As Object
            If e IsNot Nothing Then
                RaiseEvent ExternalFSCommandCall(Me, e)
            End If
            Return Nothing
        End Function



        ' <summary> 
        ' Raises the ExternalInterfaceCall event, indicating that a call has come from Flash Player. 
        ' </summary> 
        ' <param name="e">The event arguments related to the event being raised.</param> 
        Protected Overridable Function OnExternalInterfaceCall(ByVal e As ExternalInterfaceCallEventArgs) As Object
            If e IsNot Nothing Then
                RaiseEvent ExternalInterfaceCall(Me, e)
            End If
            Return Nothing
        End Function

#End Region

#Region "Event Handling"

        ' <summary> 
        ' Called when Flash Player raises the FlashCallEvent (when an External Interface call 
        ' is made by ActionScript) 
        ' </summary> 
        ' <param name="sender">The object raising the event</param> 
        ' <param name="e">The arguments for the event</param> 
        Private Sub _flashControl_FlashCall(ByVal sender As Object, ByVal e As _IShockwaveFlashEvents_FlashCallEvent)
            Dim functionCall As ExternalInterfaceCall = ExternalInterfaceSerializer.DecodeInvoke(e.request)
            Dim eventArgs As New ExternalInterfaceCallEventArgs(functionCall)
            Dim response As Object = OnExternalInterfaceCall(eventArgs)
            _flashControl.SetReturnValue(ExternalInterfaceSerializer.EncodeResult(response))
        End Sub

        Private Sub _flashControl_FSCommandCall(ByVal sender As Object, ByVal e As _IShockwaveFlashEvents_FSCommandEvent)

            OnExternalFSCommandCall(e)

        End Sub


#End Region
    End Class
End Namespace