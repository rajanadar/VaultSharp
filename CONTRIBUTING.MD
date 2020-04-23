# How to contribute

Please read these guidelines before contributing to VaultSharp:

 - [Question or Problem?](#question)
 - [Issues and Bugs](#issue)
 - [Feature Requests](#feature)
 - [Submitting a Pull Request](#pullrequest)
 - [Contributor License Agreement](#cla)


## <a name="question"></a> Got a Question or Problem?

 * If you have questions about how to use VaultSharp, please read the
[VaultSharp documentation][documentation] or ask on [Stack Overflow][stackoverflow].
 * We would like to encourage folks to have questions and clarifications on Stack Overflow for the benefit of the whole community.

GitHub issues are only for [reporting bugs](#issue) and [feature requests](#feature), not
questions or help.


## <a name="issue"></a> Found an Issue?

If you find a bug in the source code or a mistake in the documentation, you can help by
submitting an issue to the [GitHub Repository][github]. Even better you can submit a Pull Request
with a fix.

When submitting an issue please include the following information:

- A description of the issue which includes the Vault Server version & the VaultSharp Version
- The exception message and stacktrace if an error was thrown
- .NET framework details
- If possible, please include code that reproduces the issue. [DropBox][dropbox] or GitHub's
[Gist][gist] can be used to share large code samples, or you could
[submit a pull request](#pullrequest) with the issue reproduced in a new test.

The more information you include about the issue, the more likely it is to be fixed!


## <a name="feature"></a> Want a Feature?

You can request a new feature by submitting an issue to the [GitHub Repository][github]. Before
requesting a feature consider the following:

- Read the existing documentation to ensure it doesn't already exist.
- Please provide the equivalent HashiCorp's Vault API reference links
- An associated PR is even more welcom


## <a name="pullrequest"></a> Submitting a Pull Request

When submitting a pull request to the [GitHub Repository][github] make sure to do the following:

- Please adhere to the existing coding pattern in the project. It should be straight-forward to absorb.
- Run the VaultSharp.Samples Console App to ensure it runs sucessfully.
- Write appropriate code samples in the documentation. (README.md)
- Add the summary of the issue in the changelog

Read [GitHub Help][pullrequesthelp] for more details about creating pull requests.


## <a name="cla"></a> Contributor License Agreement

By contributing your code to VaultSharp you grant Raja Nadar a non-exclusive, irrevocable, worldwide,
royalty-free, sublicenseable, transferable license under all of Your relevant intellectual property rights
(including copyright, patent, and any other rights), to use, copy, prepare derivative works of, distribute and
publicly perform and display the Contributions on any licensing terms, including without limitation:
(a) open source licenses like the Apache 2.0 license; and (b) binary, proprietary, or commercial licenses. Except for the
licenses granted herein, You reserve all right, title, and interest in and to the Contribution.

You confirm that you are able to grant us these rights. You represent that You are legally entitled to grant the
above license. If Your employer has rights to intellectual property that You create, You represent that You have
received permission to make the Contributions on behalf of that employer, or that Your employer has waived such
rights for the Contributions.

You represent that the Contributions are Your original works of authorship, and to Your knowledge, no other person
claims, or has the right to claim, any right in any invention or patent related to the Contributions. You also
represent that You are not legally obligated, whether by entering into an agreement or otherwise, in any way that
conflicts with the terms of this license.

Raja Nadar acknowledges that, except as explicitly described in this Agreement, any Contribution which
you provide is on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED,
INCLUDING, WITHOUT LIMITATION, ANY WARRANTIES OR CONDITIONS OF TITLE, NON-INFRINGEMENT, MERCHANTABILITY, OR FITNESS
FOR A PARTICULAR PURPOSE.


[github]: https://github.com/RajaNadar/VaultSharp
[documentation]: http://rajanadar.github.io/VaultSharp/
[stackoverflow]: https://stackoverflow.com/questions/tagged/vault-sharp
[dropbox]: https://www.dropbox.com
[gist]: https://gist.github.com
[pullrequesthelp]: https://help.github.com/articles/using-pull-requests
